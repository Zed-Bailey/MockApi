# Mock API

mock api is an application that allows you to import or create a csv file
and generate a simple api to use while testing/developing your application


## Data

All data returned from the api is in json format.

Data returned from the endpoint is in a json dictionary format
eg. 
```json
{
  "id" : "1",
  "column1" : "value1",
  "column2" : "value2",
  "column3" : "value3"
}
```

the id key/value is the rows index in the table
this value is used when wanting to 
- get an individual row
- updating a row 
- deleting it 

## Endpoints


- **Get** `/api/data/`
  - returns all rows in the table


- **GET** `/api/data/:id`
  - returns the row with a matching id (the id is the rows index)


- **GET** `/api/data/select?where={column name}&is={value}`
  - searches rows for matching column/value pair, returns all rows that match
  - optional `limit` parameter to only return a set amount, by default all matching rows will be returned
  - eg. limit to 1 row: `/api/data/select?where={column name}&is={value}&limit=1`




## MVF

- [ ] Crud column 
  - [X] create
  - [X] read
  - [X] update
  - [ ] delete
  
- [x] Crud row
  - [X] create
  - [X] read
  - [X] update
  - [X] delete
  
- [x] Get data endpoints
  - [x] return all endpoint
  - [x] select endpoint
  - [x] return by id endpoint



## Future improvements

- look into changing the data table into a MudBlazor DataGrid
- implement an inbuilt route tester/generator 