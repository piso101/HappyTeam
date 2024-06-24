
# HappyTesla

Project made for requitment task from HappyTeam




## Authors

- [Oskar Kacała](https://www.linkedin.com/in/oskar-kacała-b986b5267/)


## Deployment

To deploy this project locally

Enter HappyBackEnd folder:
```bash
cd .\HappyBackEnd\
```

And run backend by:
```bash
dotnet run
```
Swagger will be avaiable on https://localhost:7237/index.html

Swagger is used to document Api 

Now Enter FrontEnd

Open another cmd inside of project.

Enter FrontEnd Folder:

```bash
cd .\happyteamappfront\
```


And Run FrontEnd by:

```bash
npm start
```

Frontend is avaiable on http://localhost:3000

Frontend and simple admin panel is avaiable on http://localhost:3000/admin

Password is HappyAdmin and Login is HappyPassword.


## Security
App is secured by using:
- Email verification
- Jwt tokens to login to Admin Panel
- Api Antyspam protection (by default its set to be 20 request per minute per IP)

## GuideLines
App was made for a company, specializing in tesla rental. 

Good looking website is not important so wasn't paying attention to it.

They have a few locations and client can leave the car in any one of them.

Assumptions:

Client has to pick 2 locations and 2 dates. Then Client has to verify their order by filling the form and clicking the link send to their email. 

Company has to wait till the car has come back to Unit before letting rent it to other customers. (we dont know if client is going to pickup car even if email has been verified.)

