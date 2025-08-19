Feature: Entries API layer

Scenario: Can add entry
	Given an entries request
		| Title | Description              | Date       | Distance | DistanceUnit |
		| Day 1 | The porridge was too hot | 2020-02-01 | 10.5     | 0            |
	When the entries request is sent
	Then the entries request is successful with a status code of 200
	And the entry data is stored in the database
		| Title | Description              | Date       | Distance | DistanceUnit |
		| Day 1 | The porridge was too hot | 2020-02-01 | 10.5     | 0            |
	#And there is an event on the downstream queue
