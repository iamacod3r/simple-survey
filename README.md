## Simple Survey with .net 5.0

.net 5.0 version with **n-tier** architecture is used in this software.<br />
To store the data I used **Static** objects, and to avoid any conflicts with the data add or update processes<br />
I used **'async lock'** method. Additionally, I included a test project to facilitate testing process. <br />

**Service** layer in this application is designed in a way to be able to serve to other applications in addition to API.<br />
Therefore, it can be easily applied to other projects as well.

**API Support** :
* Creating a survey
* Taking a Survey
* Getting Results of a Survey

**How to run it ?**
* First of, [download .net 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)
* Download [this repository](https://github.com/cihandokur/SimpleSurvey.git)
* Navigate to the folder where you downloaded the code. 
* Finally, type **dotnet run** command.

<br />

## How to develop this software in Production?

In order to keep the collected data (survey content, questions and answers to the survey) persistently 
I would use a database such as **PostgreSQL**. <br />
In case of extensive use of the system, to improve the performance of the database I would use a **Cache** system such as **Redis**.<br />
Also, I would use an Admin web application for the management of the survey.
<br />
