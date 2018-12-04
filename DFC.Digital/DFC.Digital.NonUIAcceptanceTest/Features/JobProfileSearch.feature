@JobProfile
Feature: List job profiles based upon relevance to the "search term" entered

Scenario: [DFC-5740] Search phrase that matches alternative title on job profile should be top of the results
	Given I have a list of all job profile with titles and alterantive title
	When I search by each alternative title for each of the  job profiles
	Then all the results returned should have the job profile with the matching alterantive tag on the first page.

Scenario: [DFC-5220] Search phrase that matches title on job profile should be top of the results
	Given I have a list of all job profile with titles and alterantive title
	When I search by each title for each of the job profiles
	Then all the results returned should have the job profile with the matching title in the first position.
