# Email Service - coding excercise 
	import data from text received via email.
	The data will either be:
	• Embedded as ‘islands’ of XML-like content
	• Marked up using XML style opening and closing tags

# Steps to run
	1. Open the solution with Visual Studio, restore NuGet Packages
	2. Build application
	3. Run application by click F5 

# API - http://localhost:VisualStudioPort/api/emailservice
	1. POST: Input the content in body content
	2. GET: will get all the results which saved in local database
	3. GET: http://localhost:60432/api/emailservice/{id} will return the result by passed id
	4. DELETE: http://localhost:60432/api/emailservice/{id} will delete the record from local database
	
# Feature
	1. Local database supported
	2. Simple UI supported

# Assumptions
	1. No security consideration. e.g. HtmlSanitizer
	2. Use GUIDs as primary key for each record instead of Integers
	3. Emails replaced with empty string in order to avoid xml syntax exception. e.g.  <Antoine.Lloyd@example.com>
	4. Use 15% as default GST rate
	5. Use JSON as return format	
