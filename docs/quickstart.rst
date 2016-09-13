.. _quickstart:

Quickstart
============


Download the Example workbook :download:`Examples.xlsb <Examples.xlsb>`

Retrieve Json from an HTTP API straight into Excel, authenticate via basic authentication:

    >>> =REQUESTS.GET("https://api.github.com/user",,, "<username>:<token>")
    https://api.github.com/user
    >>> =REQUESTS.DICT.KEYS("https://api.github.com/user#Json")
    {"login";"id";"avatar_url";"gravatar_id";"url";"html_url"; ...}
    >>> =REQUESTS.DICT.GET("https://api.github.com/user", "Json/url")
    https://api.github.com/users/Pathio
    >>> =REQUESTS.FLUSH()
    Flushed 1 key(s)

