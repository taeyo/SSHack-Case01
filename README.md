# SSHack-Case01

this repo is using normal Azure Function (not Durable Function)

TEST URL
- https://{URI}/api/Gateway?code=Kis8Cs4ZbSIcyvzP0stbjdWGNQQraj/y/fF9mnJuNsfmUvlXa8LjjQ==
  - ex : https://sshack-case01.azurewebsites.net/api/Gateway?code=Kis8Cs4ZbSIcyvzP0stbjdWGNQQraj/y/fF9mnJuNsfmUvlXa8LjjQ==

TEST POST Data : 
  ````
  {    
     "RequestID" : "aasd-asdasdas-dsadsad",
     "Command": {
         "FromTime":"2018-07-01 00:00:00",
         "ToTime":"2018-07-01 10:00:00"
     },
     "Div" : "10"
  }
  ````

Status Check URl :
- https://{URI}/api/requestid/{Request ID}?div={Divide Number}
  - ex : https://sshack-case01.azurewebsites.net/api/requestid/aasd-asdasdas-dsadsad?div=10
