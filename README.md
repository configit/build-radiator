# Build Radiator

### Installation instructions

* Publish from Visual Studio, or 
* Clone/Copy the code into a folder and host it via IIS.

### Authentication

Log in with the credentials of a user, with access to the builds you wish to poll.

### Customization

* Branch the code
* Modify layout in the [TileController](https://github.com/configit-open-source/build-radiator/blob/master/BuildRadiator/Controllers/TileController.cs)
* Modify messages used by the MessageTile in [MessageController](https://github.com/configit-open-source/build-radiator/blob/master/BuildRadiator/Controllers/MessageController.cs)

### Notes

Once running, credentials are stored in memory. If the app pool recycles you will need to log in again. You may wish to disable nightly recycling.
