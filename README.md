# Website for foundation "fundacion para el apoyo del cancer oral e infantil"
#### Video Demo:  https://youtu.be/gz0wntUkdBo
#### Description:
The final project for cs50 is a website for a foundation I'm part of, named "fundacion para el apoyo del cancer oral e infantil" who didnt have a website previously, the implementation is fairly simple with modules for user login, donation creation, static design and information and paypal donation

#### Technologies:
For the webpage I wanted to try something new, so I delved on how to make a website using aspnet, the programming languages/libraries used are:
- C#/.Net/ASPNet
- SQL server/SQL Server Management
- Scaffolding & Razor pages
- Bootstrap/CSS/Html
- Flux Ai

## How it works:
The webpage centers around showing the user what the foundation is and on encouraging the user to donate, for this a database was created on sql server which stores the different types of donations and some characteristics of them, such as:
- Name
- Paypal Link
- Location
- Type of Donation
- Impact
- Goal

Between the variables one of the key ones lies in the Paypal link (IframeLink) which is used to direct users to a paypal account configured for the foundation and the webpage, for this I sincerely debated on what the best way would be to implement the donation feature, either as a paypal link which is easier to implement but doesnt show the progress of the donation goal or directly getting involve in writing the code for the paypal api which is tougher but allows for the functionality for showing the progress of the donation goal, however as I was delving into both of this method I found [Paypal Campaigns](https://www.paypal.com/donate/buttons/manage?fromLanding=true) which allows both the progress functionality and an ease of implementation, thus I went with that method by utilizing an iframe for this link which the user can register for each donation.


#### Donation Creation/Deletion
In the same manner, the webpage handles the creation, visualization and deletion of a donation, for this a registered user can see in a sample table which donation exists and create/delete  any of them, for this the utilization of a sql server database was used, to be able to save both the registered user and the donations created dynamically.

#### Permissions & Login
As the pages for creation, deletion and visualization of the donations on a "master" level can leave risks such as change, modification or deletion of any donation, the webpage made this 3 functionalities (and thus webpages) only accesible for authorized users, this was made through the use of the scaffolding library as this libraries allows for the creation and authorization of users in the viewing and interaction of the webpage, allowing only authorized users to get to the webpage with the `[Authorize]` feature

#### Logging
In a similar manner through the use of Scaffolding and SQL a user may be able to create an account and log in, however as the foundation will only have 1 person making the donation forms, a user may only create an account if it uses an specific email

#### Donation filter
Apart from the general gallery for donations any user can see, the webpage also has a filter for donations in which the user may find donations based on criteria such as impact, goal, location, type of donation or donation recipients, between other potential filter, for this a sql query is used which is divided partially based on what the user desires and then is joined together as an sql query to look for any donations which fit that criteria

#### Design and Information
Finally for the webpage a basic design using bootstrap and some simple animations where done for the page, in a similar manner flux AI was use for the majority of the images obtained for the webpage

#### Possible Improvements
For the future of this project there may be many ways in which could improve the application such as:
* More methods of payment
* Better webpage design
* Less dependencies on paypal campaigns for pay
* Greater range of permission and confirmation in user creation

#### Contribution
For the code creation there existed use of G PT *4 throught some areas of the code, specifically elements like css and the class for html, in a more concrete manner most of the use of the tools and where they were used are written as comments inside the code, along with some of the functions specifications

#### About the code
For the code programming the main files for the code are found in:
* Pages
* Areas/Identity/Pages

#### Instructions
* To run the webpage first uncompress the vs_zipped_code
* Open the file in visual studio (not visual studio code)
* Download scaffold, aspnet, c# for visual studio
* Run the code as part of the local ip of the pc/serv er

Juan Rivera, riv-gi t 

