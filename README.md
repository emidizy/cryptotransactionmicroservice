# cryptotransactionmicroservice
A microservice that handles transaction inquiry on a crypto wallet

Requirements:
•	Visual studio 2017 or later
•	.Net core 2.2 or later
•	MSSQL Server (local or cloud hosted)
•	RabbitMQ 3.7+  (local or cloud hosted)


Description:
This project is made up of two applications; 
•	A client microservice which sends a message to an exchange to update transactions. This message was configured to be automatically sent every 5mins
•	A transaction microservice which listens for published messages on the exchange and executes a check for new client transactions upon receipt of update events and as well publish new transactions to the exchange
Endpoints available on each microservice can be found on {{baseUrl}}/swagger
Transaction records are persisted on a MsSQL database and this can be graphically accessed via an SQL Server Management Studio

Usage Guide
Follow the steps below before you run any of the projects;
•	Open the project’s appsettings file & ensure the database and RabbitMQ broker configurations are same as on your environment.
•	Open ‘’Package Manager console’’ CLI on visual studio while the project is open
•	Execute the following command on the CLI to create the application’s database
1.	update-database

Once done with the above steps, run both applications (advised to keep both applications running at same time so that you can get real time Publish - Ack events).
To manually trigger an ‘update transaction’ request, an endpoint “Transactions/update” is provided on the client microservice for such purpose.
The transaction histories can be queried on the Transaction microservice via the endpoints “Transaction/all/retrieve” and “Transaction/client/retrieve”

Assumptions:
1.	A third party cyptocurrency API exists and is expected to return transactions for a given wallet address, however given the time constraints of this implementation, I did not use a third party API for this purpose.

Work Around:
I created a function to generate mock dynamic & duplicate transactions based on the given specifications and conditions


Contact: ediala94@gmail.com





