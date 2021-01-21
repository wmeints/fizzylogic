Feature: Manage content

	You can manage content on the website as an administrator.
	You're always required to specify the following pieces of information:

	* Title
	* Content
	* Category

	The header image is optional and can be left empty. 
	The following additional business rules apply:

	* You can publish an article once. It remains published afterwards.
	* A new slug is created once for an article, the first time you save/publish it.
	* An excerpt is generated when the article is saved/published if you don't specify one yourself.

Background:

Given I am authenticated as an administrator
And I am on the management dashboard 

Scenario: Create a draft article

When I create a new article with the title "My article"
And I save the article 
Then a draft article exists with the title "My article"

Scenario: Create a published article

When I create a new article with the title "My published article"
And I publish the article 
Then a published article exists with the title "My published article"

Scenario: Publish a draft article

Given I have a draft article with the title "My draft article"
When I am editing the article with the title "My draft article"
When I publish the article
Then a published article exists with the title "My draft article"
