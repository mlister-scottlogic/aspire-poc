Feature: Entries API layer

Scenario Outline: Can't add entry with missing fields
	Given an entries request
		| Title   | Description   | Date   | Distance   | DistanceUnit |
		| <title> | <description> | <date> | <distance> | <unit>       |
	When the entries request is sent
	Then the entries request is unsuccessful with a status code of 400

	Examples: 
		| title | description                | date       | distance | unit  |
		| null  | The porridge was too hot   | 2020-02-01 | 10.5     | Miles |
		| Day 1 | The porridge was too cold  | null       | 10.5     | Miles |
		| Day 2 | The porridge was too sweet | 2020-02-01 | null     | Miles |

Scenario: Can add entry
	Given an entries request
		| Title | Description                 | Date       | Distance | DistanceUnit |
		| Day 7 | The porridge was just right | 2020-02-01 | 10.5     | Miles        |
	When the entries request is sent
	Then the entries request is successful with a status code of 200
	And the entry data is stored in the database
		| Title | Description                 | Date       | Distance | DistanceUnit |
		| Day 7 | The porridge was just right | 2020-02-01 | 10.5     | Miles        |
	And there is an event on the entries queue
		| Title | Description                 | Date       | Distance | DistanceUnit |
		| Day 7 | The porridge was just right | 2020-02-01 | 10.5     | Miles        |