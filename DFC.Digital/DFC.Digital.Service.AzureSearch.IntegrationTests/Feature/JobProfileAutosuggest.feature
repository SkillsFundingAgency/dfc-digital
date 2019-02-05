Feature: JobProfileAutosuggest returns suggestions using Azure Search Suggesters 


	Scenario: [DFC-1496 - A1] User uses a suggestion term to find suggestions agianst the <AltTitle> AND the <Title> on DIFFERENT profiles
		Given the following job profiles exist:
			| Title                     | AlternativeTitle     |
			| 4ut0c0mpl3t3              |                      |
			| DFC1496                   | DFC1496 4ut0c0mpl3t3 |
			| DFC1496 4ut1c1mpl3t3      |                      |
			| Pilot                     | Co-Pilot             |
			| General practitioner (GP) | Doc                  |
			| 4u2c2mpl3t3 DFC1496   |                      |
			| DFC1496 4u3c3mpl3t3       |                      |
		When I type the term '4ut'
		Then the result list will contain '5' suggestion(s)
		And the suggestion are listed in no specific order:
			| Title                   | AlternativeTitle     |
			| 4ut0c0mpl3t3            |                      |
			| DFC1496                 | DFC1496 4ut0c0mpl3t3 |
			| DFC1496 4ut1c1mpl3t3    |                      |
			| 4u2c2mpl3t3 DFC1496   |                      |
			| DFC1496 4u3c3mpl3t3     |                      |