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

Currently the only problem is, is that images are not deleted from the images folder.
You will have to manually delete them.

Also, tests are usually done when the application is in Development mode.
That is because when it is in development mode, the authorization is bypassed.

I have not tested everything with the authorization, because I do not have the 
time to do so. Also because I'd have to test every endpoint multiple times with
different roles.
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


t = asyncio.run(main(results, tracker))
