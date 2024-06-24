# HappyTesla

Project created as part of a recruitment task for HappyTeam

## Authors

- [Oskar Kacała](https://www.linkedin.com/in/oskar-kacała-b986b5267/)

## Deployment

To deploy this project locally:

1. Enter the `HappyBackEnd` folder:
    ```bash
    cd .\HappyBackEnd\
    ```
2. Run the backend with:
    ```bash
    dotnet run
    ```
    Swagger documentation will be available at [https://localhost:7237/index.html](https://localhost:7237/index.html).

    Swagger is used to document the API.

3. Enter the `FrontEnd` folder in another command prompt:
    ```bash
    cd .\happyteamappfront\
    ```
4. Run the frontend with:
    ```bash
    npm start
    ```
    The frontend will be available at [http://localhost:3000](http://localhost:3000).

    The frontend and a simple admin panel are available at [http://localhost:3000/admin](http://localhost:3000/admin).

    The login credentials are:
    - **Password:** HappyAdmin
    - **Login:** HappyPassword

## Security

The app is secured by using:
- Email verification
- JWT tokens for logging into the Admin Panel
- API anti-spam protection (by default, it is set to 20 requests per minute per IP)

## Guidelines

This app was created for a company specializing in Tesla rentals.

A visually appealing website was not a priority, so attention was not paid to aesthetics.

The company has a few locations, and clients can return the car to any of them.

Assumptions:

- Clients must select 2 locations and 2 dates.
- Clients must verify their order by filling out a form and clicking the link sent to their email.
- The company must wait until the car is returned before renting it to other customers (we don't know if the client will pick up the car even if the email is verified).
