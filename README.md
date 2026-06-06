# FxManifest Populator

## Description

FxManifest Populator is a tool designed to automate the process of populating fxmanifest files with metadata. It scans the specified resource directory for relevant files and generates a comprehensive fxmanifest file that includes all necessary metadata. It can be used on map resources, vehicle packs, or any other resource that requires an fxmanifest file with metadata.

You can use it for your LUA scripts but you will need to add your LUA files manually to the generated fxmanifest file.

## How to install

Download the latest release from the [Releases](https://github.com/eliott-b/fxmanifest-populator/releases) page.

## How to use

```batch
fxmanifest-populator.exe <resource_directory>
```

Your data files must be named according to the following table for the tool to recognize and include them in the generated fxmanifest file:

| Datafile Type                | Required Name(s)           |
| ---------------------------- | -------------------------- |
| `AMBIENT_PED_MODEL_SET_FILE` | `ambientpedmodelsets.meta` |
| `CARCOLS_FILE`               | `carcols.meta`             |
| `DLCTEXT_METADATA_FILE`      | `dlctext.meta`             |
| `HANDLING_FILE`              | `handling.meta`            |
| `PED_METADATA_FILE`          | `peds.meta`                |
| `VEHICLE_LAYOUTS_FILE`       | `vehiclelayouts.meta`      |
| `VEHICLE_METADATA_FILE`      | `vehicles.meta`            |
| `VEHICLE_MODEL_SET_FILE`     | `vehiclemodelsets.meta`    |
| `VEHICLE_VARIATION_FILE`     | `vehiclevariations.meta`   |

## Contributing

Contributions are welcome! If you have any suggestions or improvements, please feel free to submit a pull request or open an issue on the GitHub repository.

## License

This project is under the GNU GPL-3.0 License. See the [LICENSE](./LICENSE) file for more details.

## Credits

Tool made by [@Eliott B](https://github.com/eliott-b).
