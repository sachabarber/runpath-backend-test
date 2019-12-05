OVERVIEW

- There is a simple web api which holds the album routes
- There are 2 dedicated REST clients that deal with fetching the data for the excercise
- There is a cache client, which will either use the cache for previously fetched data, or will fetch the data again if the cache should be invalidated
- There are integration and unit tests using XUnit