Feature: DuckDuckGo Search
	In order to find what I want
	As a searcher of knowledge
	I want to find content regarding a subject

Scenario: Use DuckDuckGo to search using a specific search term
	Given I have entered "Stephen Herrick" in the search text box
	When I click the search button
	Then search results display

Scenario Outline: Use DuckDuckGo to search with multiple search terms
	Given I have entered <SearchTerm> in the search text box
	When I click the search button
	Then search results display

	Examples: 
	| SearchTerm               |
	| Software quality         |
	| Unit tests               |
	| Software Test Automation |
	| Funny software bugs	   |