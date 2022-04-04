# Mock API

mock api is an application that allows you to import or create a csv file
and generate a simple api to use while testing/developing your application


## Endpoints

All data returned from the api is in json format.


**GET** /

returns all the rows


**GET** /select?where={column name}&is={value}

returns all the rows with a matching column value



## MVF

- [ ] add/edit/delete a column
- [ ] add/edit/delete a row
- [ ] edit a cell
- [ ] mark column as un-editable with right click menu on column
- [ ] query data


## Future improvements

- look into changing the data table into a MudBlazor DataGrid
- implement an inbuilt route tester/generator 