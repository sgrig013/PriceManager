# PriceManager Test application
  - **PriceManager.cs**: Implements value monitoring. Includes Trigger inner class that clients use to create and subscribe to price triggers.
  - **TestClient.cs**: An example Trigger client. Keeps track of how many times an alert has fired, and is used in the unit tests.
  - **Program.cs**: A simple console app that uses the TestClient to for a hands on usage example.
  - **PriceManagerTests.cs**: Unit test suite project, which tests common use cases.
