Feature: Entries API layer

Scenario: Can add entry
	Given some request
	When the entries request is sent
	Then the entries request is successful with a status code of 200
	#And the data is stored in the database
	#And there is an event on the downstream queue
