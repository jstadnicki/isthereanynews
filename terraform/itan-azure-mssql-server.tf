resource "azurerm_mssql_server" "itan-mssql-server" {
  administrator_login = var.itan-admin-mssql
  administrator_login_password = random_password.itan-mssql-admin-password.result
  location = var.location
  name = "itan-mssql-server"
  resource_group_name = var.resource-group-name
  version = "12.0"

  depends_on = [
    azurerm_resource_group.itan-west-europe-resource-group]
}
