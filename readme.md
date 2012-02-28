This project makes it possible to crawl a website and output details about the crawled pages to for example an SQL server database, or in a NUnit test suite.

For example, it is possible to set the program to crawl 1000 pages as one NUnit test, and fail the test if there are HTTP status codes different from 200 OK.

Quick start
----

Get the latest build here: http://test.uncas.dk/WebTester.htm

Edit Uncas.WebTester.ConsoleApp.exe.config to your settings:

* maxPages: the max number of pages to crawl

* url: start url

* matches: the crawler only visits pages that starts with these urls

* webTesterConnectionString: the connection string to a SQL server

Run Uncas.WebTester.ConsoleApp.exe without arguments which uses the values from the config file.

Or use command-line input syntax: 

Uncas.WebTester.ConsoleApp.exe -url http://www.google.com -maxPages 100 -connectionString "Server=...;Database=...;Integrated Security=true" -matches http://www.google.com

Finally the results van be viewed in the database.