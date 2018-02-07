# Digital First Careers � Find a career website
Website for career information advice and guidance.

Build status

List of dependencies in a table

|Item	|Purpose|
|-------|-------|
|Sitefinity ||	
|SQL Azure||	
|REDIS	|Needed for local?|
|Azure Search||	
|Bing spell check	||
|Course finder	||
|LMI for all||	





## Running Locally
Introductory text about running local�
Then a section for each dependency, stating what someone would need to do to stand up the dependency, and then config settings which would need to be updated in the repo.

## Sitefinity 

Project - DFC.Digital.Web.Sitefinity

In core of our solution is the Sitefinity CMS. In order our solution to run as prerequisite the fully fledged licenced version of Sitefinity have to be installed, configured and running.
�Installation: Get started with Sitefinity CMS�
https://docs.sitefinity.com/installation
Once you have Sitefinity running you can take next step of plugging Sitefinity into our solution. 

First of all you have to set the connection string to the one you are using.

And migrate the values generated by your Sitefinity installation into tokenised settings of our solution.



| Config File   | Key                                       | Token                       | Example value             |
|-------------| ----------------------------------------- |-----------------------      |:----------------:|
|DataConfig.config |	connectionString |	\_\_connectionString\_\_	| e.g. "data source=.;UID=sitefinityUser;PWD=sitefinityPWD;initial catalog=sitefinityDB"           | 
|ApplicationInsights.config |	InstrumentationKey |	\_\_appInsightsKey\_\_	| It's a GUID in following format "1abc2345-abc3-41g4-96a9-1ab2cd34ef56"             | 
|SecurityConfig.config |	relyingParties - key |	\_\_securityRelyingParties\_\_	| "12ABC3DCC1C686346DC40B816ADC126C011E99210BD8A8AE735CD9A54EDF5188"             | 
|SecurityConfig.config |	securityTokenIssuers - key |	\_\_securityTokenIssuers\_\_	| "12ABC3DCC1C686346DC40B816ADC126C011E99210BD8A8AE735CD9A54EDF5188"             | 
|SecurityConfig.config |	decryptionKey	| \_\_sitefinitySecurityDecryptionKey\_\_	| "0A1B12AB6CB969A878C3F06B438B6491D79A6CAD22A3982F9497A1AE21CE9740"             | 
|SecurityConfig.config |	/securityConfig/applicationRoles/role - [XML] |	\_\_sitefinitySecurityRolesSection\_\_	| \<applicationRoles\> section can be replaced              | 
|SecurityConfig.config |	validationKey |	\_\_sitefinitySecurityValidationKey\_\_	| "90A36AB52A6E2D6558AA4AB1CFCBFE15AF8EC6A77015DC53E8CCC865E1D75EC0725E8FB8D31EED4F50FE3D5B050FC7E90508E036A9E5C5B84C17B22787285FC1"             | 

After restarting of the application our content types should be recreated by the installed AddOns.
Please, be aware that PreSearchFilters AddOn should be installed first as they are used as related data in the main content type JobProfile. 


This will allow you to populate the database with relevant content.

## SQL Azure

## REDIS
Redis caching is not used on local development machines. But it is used when our solution is deployed in Azure Load Balanced environment. 
## Azure Search

## Bing spell check

## Course finder

## LMI for all