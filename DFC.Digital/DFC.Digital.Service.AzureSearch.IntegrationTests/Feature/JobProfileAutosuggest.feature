Feature: JobProfileAutosuggest returns suggestions using Azure Search Suggesters 


	Scenario: [DFC-1496 - A1] User uses a suggestion term to find suggestions agianst the <AltTitle> AND the <Title> on DIFFERENT profiles
		Given the following job profiles exist:
			| Title                     | AlternativeTitle           |
			| Counsellor                | Therapist, psychotherapist |
			| Money adviser             | Debt counsellor            |
			| Co-ordinator              | Extra End                  |
			| Pilot                     | Co-Pilot                   |
			| General practitioner (GP) | Doc                        |
		When I type the term 'Cou'
		Then the result list will contain '4' suggestion(s)
		And the suggestion are listed in no specific order:
			| Title         | AlternativeTitle           |
			| Counsellor    | Therapist, psychotherapist |
			| Money adviser | Debt counsellor            |
			| Co-ordinator  | Extra End                  |
			| Pilot         | Co-Pilot                   |
