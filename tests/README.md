## ⚠️READ THIS ⚠️
These tests were written during development as a practical way of answering the question:
> "hey, does this service *actually* work?"

In that sense, these tests currently break the point of a unit test by not only testing the services, but also the APIs (which this code has no control over). In a nutshell, the tests make actual HTTP requests.

You can track refactoring of these tests on [issue #9](https://github.com/bcanseco/common-bot-library/issues/9).
