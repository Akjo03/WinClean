# Versioning

**WinClean is not following Semantic Versioning altough we use the format {MAJOR}.{MINOR}.{PATCH}-{PRE}**
| Version Part | Description |
| - | - |
| **MAJOR** | Is 0 as long as not every part has been implemented from the initial planning. You can find the parts from the initial planning phase under [PARTS.md](PARTS.md). <br/> Is 1 after every part of the inital planning has been implemented. Afterwards it increases for every major change of WinClean. |
| **MINOR** | Begins at 0 and increases for every part added. Be that in the pre-release phase (major version 0) or afterwards. |
| **PATCH** | Begins at 0 and increases for every patch (multiple bug fixes together). This can also include small feature changes. |
| **PRE** | Used if the current version is not production ready but needs a release for testing. Increased if there are multiple pre-releases |

NOTE: If you make a release on Github use the pre-release tag only before v0.1.0 or if PRE is set.

## Examples
| Version | Description |
| - | - |
| WinClean v0.1.1 | Before full release but production ready. First part of WinClean with 1 patch implemented. |
| WinClean v0.3.2 | Before full release but production ready. First three parts of WinClean with 2 patches implemented. |
| WinClean v0.4.0-1 | Before full release and not production ready. First four parts with no patches implemented. First pre-release of this version. |
| WinClean v1.0.1-3 | After full release but not production ready. All parts from initial planning phase implemented with 1 patch. Third pre-release of this version. |
| WinClean v1.0.2   | After full release and production ready. All parts from initial planning phase implemented with 2 patches. |
| WinClean v1.1.0-1 | After full release but not production ready. All parts from initial planning phase + 1 additional part implemented with 1 patch. First pre-release of this version. |
| WinClean v1.2.0   | After full release and production ready. ALl parts from initial planning phase + 2 additional parts implemented with no patches. |
| WinClean v2.0.0   | After full release and production ready. All parts from initial planning phase implemented. Ground braking changes since latest v1 version implemented. No patches yet. |
| WinClean v2.1.0-2 | After full release but not production ready. All parts from initial planning phase + all the parts from v1 to v2 + 1 additional part implemented with no patches. Second pre-release of this version. |
| WinClean v2.1.2   | After full release and production ready. All parts from initial planning phase + all the parts from v1 to v2 + 1 addition part implented with two patches. |