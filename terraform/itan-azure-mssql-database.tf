resource "azurerm_mssql_database" "itan-mssql-database" {
  name = "itan"
  server_id = azurerm_mssql_server.itan-mssql-server.id
  sku_name = "basic"
  max_size_gb = "2"

  depends_on = [
    azurerm_mssql_server.itan-mssql-server]
}
// login
resource "null_resource" "itan-mssql-logins-itan-reader" {
  provisioner "local-exec" {
    command = "Invoke-Sqlcmd -ServerInstance ${azurerm_mssql_server.itan-mssql-server.fully_qualified_domain_name} -Database master -Username ${var.itan-admin-mssql} -Password '${random_password.itan-mssql-admin-password.result}' -Query 'CREATE LOGIN [${var.itan-reader-mssql}] WITH PASSWORD=${random_password.itan-mssql-reader-password.result}'"
    interpreter = ["PowerShell", "-Command"]
  }
  depends_on = [azurerm_sql_firewall_rule.itan-mssql-server-firewall-rule, azurerm_mssql_database.itan-mssql-database]
}
resource "null_resource" "itan-mssql-logins-itan-writer" {
  provisioner "local-exec" {
    command = "Invoke-Sqlcmd -ServerInstance ${azurerm_mssql_server.itan-mssql-server.fully_qualified_domain_name} -Database master -Username ${var.itan-admin-mssql} -Password '${random_password.itan-mssql-admin-password.result}' -Query 'CREATE LOGIN [${var.itan-writer-mssql}] WITH PASSWORD=${random_password.itan-mssql-reader-password.result}'"
    interpreter = ["PowerShell", "-Command"]
  }
  depends_on = [azurerm_sql_firewall_rule.itan-mssql-server-firewall-rule, azurerm_mssql_database.itan-mssql-database]
}
// user
resource "null_resource" "itan-mssql-user-itan-reader" {
  provisioner "local-exec" {
    command = "Invoke-Sqlcmd -ServerInstance ${azurerm_mssql_server.itan-mssql-server.fully_qualified_domain_name} -Database ${azurerm_mssql_database.itan-mssql-database.name} -Username ${var.itan-admin-mssql} -Password '${random_password.itan-mssql-admin-password.result}' -Query \"CREATE USER [${var.itan-reader-mssql}] FOR LOGIN [${var.itan-reader-mssql}] WITH DEFAULT_SCHEMA=[dbo];exec sp_addrolemember N'db_datareader', N'${var.itan-reader-mssql}';\""
    interpreter = ["PowerShell", "-Command"]
  }
  depends_on = [null_resource.itan-mssql-logins-itan-reader]
}
resource "null_resource" "itan-mssql-user-itan-writer" {
  provisioner "local-exec" {
    command = "Invoke-Sqlcmd -ServerInstance ${azurerm_mssql_server.itan-mssql-server.fully_qualified_domain_name} -Database ${azurerm_mssql_database.itan-mssql-database.name} -Username ${var.itan-admin-mssql} -Password '${random_password.itan-mssql-admin-password.result}' -Query \"CREATE USER [${var.itan-writer-mssql}] FOR LOGIN [${var.itan-writer-mssql}] WITH DEFAULT_SCHEMA=[dbo];exec sp_addrolemember N'db_datareader', N'${var.itan-writer-mssql}';exec sp_addrolemember N'db_datawriter', N'${var.itan-writer-mssql}';\""
    interpreter = ["PowerShell", "-Command"]
  }
  depends_on = [null_resource.itan-mssql-logins-itan-writer]
}
