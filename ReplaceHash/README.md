
# Task 2. Blind SQL injection

Purpose of the task:
1. Learn to find vulnerable code chunks
2. Learn to exploit SQL injection in order to understand the risks
3. Learn to fix SQL injection vulnerabilities

Task sequence:
Given an app with REST services running on docker server image: GET/users, POST/users, POST/login, show the proof of vulnerability by logging in as administrator
App must response "Logged in OK" in API message 

/*Consider you do not know salt value.*/

Tasks:
1. Exploiting
   a) Identify vulnerable service
   b) Create user
   c) Write your own application that is capable to extract your new user hash value
   d) Replace administrator password hash with yours and try to login (could be done with single “command”)
2. Try automated tools like "sqlmap" or any other
3. Try find and edit app server code in order to fix SQL injection vulnerability.


How to launch an application:
1. Install docker-compose
2. Download this task folder from Moodle
3. Launch application using docker composer: 
  ```
  docker-compose up
  ```

  
In order to fix server source code, docker cache clean must be done (in other case old server code will be used from cache):
  ```
  docker system prune -a
  ```
  
Sample POST request to the application:
Linux: curl --header "Content-Type: application/json" --data '{"userName":"admin","password":"guessme"}' http://localhost:8080/login
Windows: curl --header "Content-Type: application/json" --data {\"userName\":\"admin\",\"password\":\"guessme\"} http://localhost:8080/login

Sample POST reequest for new user:

curl --header "Content-Type: application/json" --data '{"userName":"alex1","userFName":"Alexander","userLName":"Bob","password":"guessme"}' http://localhost:8080/users
curl --header "Content-Type: application/json" --data {\"userName\":\"alex1\",\"userFName\":\"Alexander\",\"userLName\":\"Bob\",\"password\":\"guessme\"} http://localhost:8080/users

Sample GET request for checking if user exists:
curl "http://localhost:8080/users?username=admin"
