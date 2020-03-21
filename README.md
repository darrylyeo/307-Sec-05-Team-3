# CampusNow (CSC 307, Section 5, Team 3, Cal Poly, Winter 2020)

[![Build status](https://dev.azure.com/jasonjen0209/campusnow/_apis/build/status/campusnowbackend%20-%20CI)](https://dev.azure.com/jasonjen0209/campusnow/_build/latest?definitionId=1)

CampusNow is a web application that allows users to browse, search for, and post events that are occurring near them. The app uses a street map and pins to allow users to easily visualize where events are taking place, as well as a side bar that provides a list of upcoming events and an interface to perform the CRUD actions (post, view, edit, and delete events).

## Tech Stack

### Front-end: [Svelte](https://svelte.dev) (via Rollup and Node.js)
* View components such as `<Map>`, `<Event>`, and `<Login>` are defined in .svelte files, including HTML markup, CSS styles/animations, and JavaScript functionality
* The client-side JavaScript `fetch()` API makes HTTP requests to the REST API backend
* The components and JavaScript code are compiled into static HTML/CSS/JavaScript assets via Svelte's Node.js Rollup process

### Maps API: [Leaflet.js](https://leafletjs.com) with [OpenStreetMap](https://www.openstreetmap.org/about)
* Front-end JavaScript library, Leaflet.js, provides utility methods for rendering an interactive map, pins, and popups
* Map data is served from OpenStreetMap

### Back-end: ASP.NET Web API framework
* DTOs transfer data between client and backend
* controller receives requestDTO from frontend and passes data into corresponding repository
* repository gets or sets data in sql db, returns data back to controller
* controller returns data from repo as DTO to client

### CI / Deployment: Microsoft Azure
* azure automated builds triggered at every push to backend repo
* pull only backend solution (includes backend, tests, DTOs) from repo
* restores dependencies using nuget restore
* builds entire backend solution
* runs all test cases in tests project only if build success
* deploys to azure app service campusnowbackend.azurewebsites.net only when builds and tests pass

## Getting Started

* Svelte documentation: https://svelte.dev

* MSTest tutorial: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

* JavaScript `fetch()` API documentation: https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API

* asp.net web api https://docs.microsoft.com/en-us/aspnet/web-api/overview/getting-started-with-aspnet-web-api/tutorial-your-first-web-api

* REST API documentation page: http://api.campusnow.tech/help

* Leaflet.js documentation page: https://leafletjs.com/

* Connecting to SQL: https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient?view=netframework-4.8

## Project UML Diagram

[ProjectUML.pdf](https://github.com/darrylyeo/307-Sec-05-Team-3/files/4339842/ProjectUML.pdf)

## Task Flow Diagram

[TaskFlow.pdf](https://github.com/darrylyeo/307-Sec-05-Team-3/files/4339809/TaskFlow.pdf)
