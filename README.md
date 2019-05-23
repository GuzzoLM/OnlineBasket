# OnlineBasket

Motivation

*Part 1:*
Your company has decided to create a new line of business.  As a start to this effort, they’ve come to you to help develop a prototype.  It is expected that this prototype will be part of a beta test with some actual customers, and if successful, it is likely that the prototype will be expanded into a full product.
Your part of the prototype will be to develop a Web API that will be used by customers to manage a basket of items. The business describes the basic workflow is as follows:
This API will allow our users to set up and manage an order of items.  The API will allow users to add and remove items and change the quantity of the items they want.  They should also be able to simply clear out all items from their order and start again.
The functionality to complete the purchase of the items will be handled separately and will be written by a different team once this prototype is complete.  
For the purpose of this exercise, you can assume there’s an existing data storage solution that the API will use, so you can either create stubs for that functionality or simply hold data in memory.
Feel free to make any assumptions whenever you are not certain about the requirements, but make sure your assumptions are made clear either through the design or additional documentation.

*Part 2:*
Create a client library that makes use of the API endpoints created in Part 1.  The purpose of this code to provide authors of client applications a simple framework to use in their applications.

# API Structure

1. Products API
This API manages the products that can be added on the baskets. It allows the user to search for products, create, update and delete.

2. Baskets API
This API manages the baskets. It allows to search for all the current uer's baskets, create a new one, or delete.

3. ProductGroups API
This API manages the groups of products inside a basket. If the user adds two "Red Shirts", for instance, they form a group of products, instead of beeing two separated products.

There is a swagger UI that allows experimentation with the API. It requires a login, but when the user first access it can put any username and password and these credentials will be created. The authentication is in request format and doesn't use client_id and client_secret, so they can be just ignored.

# Client
The client has methods to call all the APIs. It enables other applications to easily integrate with the API.
(I should've done at least a console application to make it easier to test, but run out of time)
