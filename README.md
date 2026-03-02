# MediaControlMCP

MCP server for your stochastic parrot/AI partner(!) to talk to your Windows media players.

- Create a folder .local\bin in your home directory and add it to your `PATH`
- Build this using the PowerShell script and add "`MediaControlMCP.exe`" to your MCP arsenal

# Example for Claude Desktop (`claude_desktop_config.json`)

```json
{
  "mcpServers": {
    "mediaControl": {
      "command": "MediaControlMCP.exe"
    }
  }
}
```
