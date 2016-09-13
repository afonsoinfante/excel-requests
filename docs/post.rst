.. _post:

POST
====


POST Json via HTTP:

Create an empty payload dictionary

    >>> =REQUESTS.DICT.CREATE("payload")
	payload


Set payload data

    >>> =REQUESTS.DICT.SET("payload", "url", "https://www.pathio.com")
	payload#url


Do the POST

    >>> =REQUESTS.POST("https://httpbin.org/post",,"payload")
    https://httpbin.org/post


Get the response status code

    >>> =REQUESTS.DICT.GET("https://httpbin.org/post","StatusCode")
    201
