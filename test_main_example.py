"""
Welcome to my test script.

I didn't get pytest to work so I wrote my own.
All the custom function required to run this are located in 'test_functions.py'

To run the script, run the command `python <file.py> stdout`
To see what was printed, run the command `python <file.py> stdout`
where <file.py> is your python file.

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
    tracker.remove("some", thing)
`
"""


import requests
from test_functions import *
import asyncio


tracker = Tracker()
results = TestResults()


def add_nums(a, b):
    return a + b


@results.add_test
async def test_add_nums():
    assert add_nums(1, 1) == 2, "Add nums failed!"
    assert add_nums(1, 2) != 2, "Add nums failed!"


@results.add_test
async def test_posts():
    r = requests.get("https://jsonplaceholder.typicode.com/posts")
    r = r.json()

    assert len(r) == 100, "Not 100 posts!"


@results.add_test
async def create_data():
    data = {"id": 1, "message": "Heyyy"}
    tracker.add("data", data)


@results.add_test
async def remove_data():
    data = tracker.get_latest("data")
    tracker.remove("data", data)


t = asyncio.run(main(results, tracker))
