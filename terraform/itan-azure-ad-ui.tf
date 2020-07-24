resource "azuread_application" "example-ui" {
  name = "itan-ui2"
  owners = ["65748239-c3de-4eaf-9379-ef295093119e"]
  type = "webapp/api"
  identifier_uris = ["https://isthereanynewscodeblast.onmicrosoft.com/ui2"]
  reply_urls = ["http://localhost:4200"]
  oauth2_allow_implicit_flow = false

  required_resource_access {
    resource_app_id = azuread_application.example.application_id

    dynamic "resource_access" {
      iterator = item
      for_each = azuread_application.example.oauth2_permissions

      content {
        id = item.value["id"]
        type = "Scope"
      }
    }
  }
}

