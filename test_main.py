"""
Welcome to my test script.

I didn't get pytest to work so I wrote my own.
All the custom function required to run this are located in 'test_functions.py'

To create a new test:
1. Create a new async function. 
2. Add the '@results.add_test' decorator to it.
3. Name it something descriptive.
4. Test whatever needs to be tested.
5. Make sure you use 'assert' to do the test.
6. Profit

Use the tracker!
Whenever you add a "something" which needs to be deleted after the test is finished,
add it to the tracker.
It is added to the tracker by doing: tracker.add(<group>, <any data>)

After the test is done, it will display everything that is tracked and not deleted.

To delete the something from the tracker again, do: tracker.remove(<group>, <exact data>)
The <exact data> needs to match the data you want to remove.

So basicly

adding data
`
    data = create_data()
    tracker.add("some", data)
`

getting data and removing it
`
    data = tracker.get_latest("some")
    remove_data()
    tracker.remove("some", data)
`

Currently the only problem is, is that images are not deleted from the images folder.
You will have to manually delete them.

Also, tests are usually done when the application is in Development mode.
That is because when it is in development mode, the authorization is bypassed.

I have not tested everything with the authorization, because I do not have the 
time to do so. Also because I'd have to test every endpoint multiple times with
different roles.
"""


import asyncio
import math
from test_functions import *
from main import app
from pathlib import Path
from fastapi.testclient import TestClient
import sys
import json

# Printing stuff for cool reasons, also to let the user know that it's actually doing something :D
sys.stdout.write("\rStarting..")
sys.stdout.flush()


sys.stdout.write("\rStarting....")
sys.stdout.flush()


client = TestClient(app)

tracker = Tracker()
results = TestResults()


@results.add_test
async def read_account():
    response = client.get("/api/auth/me")

    assert response.status_code == 200, "Unauthorized! (Probably) Are you in development mode?"


@results.add_test
async def create_image():
    """Should create new image"""

    _test_upload_file = Path("..", "base_img.png")
    _files = {"image": ("base_img.png", _test_upload_file.open("rb"), "image/png")}

    response = client.post("/api/images?name=Test", files=_files)
    image = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, image["detail"]

    tracker.add("image", image)

    assert response.status_code == 200, "Image was not uploaded currently"
    assert image.get("name") == "Test", "Incorrect name was returned."
    assert is_valid_uuid(image.get("id")), "Invalid UUID: " + image.get("id")


@results.add_test
async def create_item():
    """Should create new item"""

    image = tracker.get_latest("image")

    new_item = {
        "name": "Testing Item",
        "description": "Description for the test.",
        "image_id": image.get("id"),
        "price": 12500,
        "is_hidden": False,
        "max_claims": 80,
    }

    response = client.post("/api/items", json={**new_item})
    item = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, item["detail"]

    tracker.add("item", item)

    assert response.status_code == 200, "Item was not created"
    assert check_item(item, new_item), "Incorrect item was returned"


@results.add_test
async def read_item():
    """Should read item"""
    existing_item = tracker.get_latest("item")

    response = client.get("/api/items/" + str(existing_item.get("id")))
    item = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, item["detail"]
    assert response.status_code == 200, "Item was not found"
    assert check_item(item, existing_item), "Incorrect item was returned"
    assert item["view_count"] == 1, "No 'view' was added"


@results.add_test
async def update_item():
    """Should update item"""
    old_item = tracker.get_latest("item")
    image = tracker.get_latest("image")

    update_item = {
        "id": old_item.get("id"),
        "name": "Testing Item but edited",
        "description": "Description for the test but edited.",
        "image_id": image.get("id"),
        "price": 15000,
        "is_hidden": True,
        "max_claims": 4,
    }

    response = client.patch("/api/items", json={**update_item})
    item = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, item["detail"]

    tracker.remove("item", old_item)
    tracker.add("item", item)

    assert response.status_code == 200, "Item was not updated"
    assert check_item(item, update_item), "Item was incorrectly updated"


@results.add_test
async def delete_image_fail_on_item():
    """Should not delete image as it is in use by items"""

    image = tracker.get_latest("image")
    response = client.delete(f"/api/images/" + str(image.get("id")))
    result = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, result["detail"]
    assert response.status_code == 400, "Image was deleted or something else went wrong."


@results.add_test
async def create_items():
    """Should create new item"""

    image = tracker.get_latest("image")
    new_item = {
        "name": "",
        "description": "",
        "image_id": image.get("id"),
        "price": 100,
        "is_hidden": False,
        "max_claims": 1,
    }

    names = ["test2", "cheese", "geese", "pokémon"]
    descriptions = ["I enjoy testing", "test desc", "pokéball", "woof"]

    for i in range(4):
        new_item["name"] = names[i]
        new_item["description"] = descriptions[i]

        response = client.post("/api/items", json={**new_item})
        item = json.loads(response.content.decode("utf-8"))

        assert response.status_code != 401, item["detail"]

        tracker.add("item", item)

        assert response.status_code == 200, "Item was not created"
        assert check_item(item, new_item), "Incorrect item was returned"


@results.add_test
async def search_items():
    """Should search correct item"""

    searches = [
        "test",
        "eese",
        "desc",
        "desc&is_hidden=true",
        "é",
        "",
        "&is_hidden=false",
    ]
    result_count = [3, 2, 2, 1, 2, 5, 4]

    for i in range(len(result_count)):
        response = client.get(f"/api/items?search={searches[i]}")
        items = json.loads(response.content.decode("utf-8"))

        assert response.status_code != 401, items["detail"]
        assert response.status_code == 200, "Error while searching"
        assert len(items) == result_count[i], \
            f"Searchresult of '{len(items)}' was not the expected '{result_count[i]}' while searching for '{searches[i]}'"


@results.add_test
async def delete_all_items():
    """Should delete all tracked items"""

    items = tracker.get("item").copy()

    for item in items:
        response = client.delete("/api/items/" + str(item.get("id")))

        assert response.status_code != 401, item["detail"]
        assert response.status_code == 200, "Item was not deleted"

        tracker.remove("item", item)


@results.add_test
async def delete_all_images():
    """Should delete all tracked images"""

    images = tracker.get("image").copy()

    for image in images:
        response = client.delete("/api/images/" + str(image.get("id")))
        result = json.loads(response.content.decode("utf-8"))

        assert response.status_code != 401, result["detail"]
        assert response.status_code == 200, "Image was not deleted"

        tracker.remove("image", image)


results.add_test(create_image)
results.add_test(create_item)


@results.add_test
async def create_claim_fail_no_item():
    """Should fail to create claim"""

    new_claim = {"item_id": "-1", "buyer": "MrFake#1234"}

    response = client.post("/api/claims", json={**new_claim})
    result = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, result["detail"]
    assert response.status_code == 404, "Item was not not found"


@results.add_test
async def create_claim():
    """Should create claim"""

    item = tracker.get_latest("item")
    new_claim = {"item_id": item.get("id"), "buyer": "MrFake#1234"}

    response = client.post("/api/claims", json={**new_claim})
    claim = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, claim["detail"]
    assert response.status_code == 200, "Claim failed to create"
    assert claim["buyer"] == new_claim["buyer"], "Buyers are not the same"

    tracker.add("claim", claim)


@results.add_test
async def read_one_claims():
    """Should find 1 claim"""

    response = client.get("/api/claims")
    claims = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, claims["detail"]
    assert len(claims) == 1, f"Found {len(claims)} claims instead of the expected 1"


@results.add_test
async def delete_item_and_delete_claim():
    """Should delete item"""

    item = tracker.get_latest("item")
    response = client.delete(f"/api/items/" + str(item.get("id")))
    result = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, result["detail"]
    assert response.status_code == 200, "Item was deleted."

    tracker.remove("item", item)
    tracker.remove("claim", tracker.get_latest("claim"))


@results.add_test
async def read_zero_claims():
    """Should find 0 claim"""

    response = client.get("/api/claims")
    claims = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, claims["detail"]
    assert len(claims) == 0, f"Found {len(claims)} claims instead of the expected 0"


@results.add_test
async def create_sale_fail_no_item():
    """Should fail to create claim"""

    new_sale = {"item_id": 0, "buyer": "string", "description": "string", "price": 0}

    response = client.post("/api/sales", json={**new_sale})
    result = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, result["detail"]
    assert response.status_code == 404, "Item was not not found"


results.add_test(create_item)


@results.add_test
async def create_sale():
    """Should create sale"""

    item = tracker.get_latest("item")
    new_sale = {
        "item_id": item.get("id"),
        "buyer": "MrFake#1234",
        "description": "It was such a bad item I had to give it away for free.",
        "price": 0,
    }

    response = client.post("/api/sales", json={**new_sale})
    sale = json.loads(response.content.decode("utf-8"))

    tracker.add("sale", sale)

    assert response.status_code != 401, sale["detail"]
    assert response.status_code == 200, "Sale failed to create"
    assert check_sale(sale, new_sale), "Incorrect sale was returned"


@results.add_test
async def delete_item_fail_on_sale():
    """Should not delete item"""

    item = tracker.get_latest("item")
    response = client.delete(f"/api/items/" + str(item.get("id")))
    result = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, result["detail"]
    assert response.status_code == 405, "Item was not not deleted."


@results.add_test
async def read_zero_unsold_items():
    """Should find 0 items"""

    response = client.get(f"/api/items?is_sold=false")
    items = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, items["detail"]
    assert response.status_code == 200, "Error while searching"
    assert len(items) == 0, f"Searchresult of '{len(items)}' was not the expected 0"


@results.add_test
async def delete_sale():
    """Should delete sale"""

    sale = tracker.get_latest("sale")

    response = client.delete("/api/sales/" + str(sale.get("item_id")))
    result = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, result["detail"]
    assert response.status_code == 200, "Sale was not deleted"

    tracker.remove("sale", sale)


@results.add_test
async def delete_item():
    """Should delete item"""

    item = tracker.get_latest("item")
    response = client.delete(f"/api/items/" + str(item.get("id")))
    result = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, result["detail"]
    assert response.status_code == 200, "Item was not deleted."

    tracker.remove("item", item)


@results.add_test
async def delete_image():
    """Should delete image"""

    image = tracker.get_latest("image")
    response = client.delete(f"/api/images/" + str(image.get("id")))
    result = json.loads(response.content.decode("utf-8"))

    assert response.status_code != 401, result["detail"]
    assert response.status_code == 200, "Image was not deleted."

    tracker.remove("image", image)


t = asyncio.run(main(results, tracker))
