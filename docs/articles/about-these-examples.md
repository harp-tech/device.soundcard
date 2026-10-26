# About these examples

The example articles demonstrate how to use the device in Bonsai. Each article walks you through a specific task, such as acquiring data, controlling peripherals, or configuring the device. When additional hardware is required, the article includes a link to the relevant connection instructions.

At the start of each article, you'll find a complete example workflow that demonstrates everything covered in that article. You can copy the workflow using the clipboard icon that appears when you hover over the image, then paste it directly into the Bonsai editor to get started.

The example workflow is divided into sections using annotation nodes (# symbols), each with a title. Each section is explained in the article under the same title. The article sections guide you through configuring the properties of the complete workflow you copied or building the workflow section by section, adding each operator individually.

We can communicate with Harp devices through Bonsai by sending commands and receiving events. As a result, most example workflows are built using three types of building blocks:

- The Harp device pattern that sets up the device, present in every workflow
- Blocks that send commands to the device
- Blocks that read events from the device

## Harp device pattern

The [Harp device pattern](https://harp-tech.org/articles/operators.html#device-pattern) is a building block that is required in every Harp workflow. It initializes the device, logs data, and allows you to send commands as well as receive messages from a Harp device according to the Harp [communication protocol](https://harp-tech.org/protocol/BinaryProtocol-8bit.html). You'll see it in the first section of the example workflow, and, because it is always the same, it is explained here and not in each example.

![Harp device pattern](../images/workflowbasics-devicepattern.svg)

The block is made up of:

- A [`BehaviorSubject`] subject operator that collects all the commands that are sent to it from elsewhere in the workflow and forwards them to the device. It is called `Device Commands` in all workflows.
- A [`Device`] operator that establishes the serial communication link with the device.
- A [`MessageWriter`] operator that saves messages or data from the device. In device-specific packages, this is replaced by a dedicated [`DeviceDataWriter`] operator named after the device.
- A [`PublishSubject`] operator that broadcasts messages received from the device, allowing other sections of the workflow to subscribe to them. It is called `Device Events` in all workflows.

> [!NOTE]
> This article uses the generic operators from the `Bonsai.Harp` package to better illustrate the concepts. When following the examples in the later articles, make sure to select the device-specific versions from the Bonsai [Toolbox](https://bonsai-rx.org/docs/articles/editor.html?tabs=mouse-controls#toolbox). For example, `Device (Harp.SoundCard)` from the `Harp.SoundCard` package instead of the generic `Device (Harp)` operator or another device's version. When correctly selected, the names of these operators in the workflow panel change to reflect the device name.

## Send commands to device

Some blocks are used to format commands and send them to the device. Sending a command requires a trigger, which can be generated in different ways in Bonsai. The examples in this guide use keyboard keys as triggers.

![Send commands to device](../images/workflowbasics-sendcommands.svg)

The block is made up of:

- A [`KeyDown`] operator that triggers the next operator to execute when a specific key is pressed.
- A [`CreateMessage`] operator that formats the command to send to the device.
- A [`MulticastSubject`] operator that sends the command to the `Device Commands` subject established in the device pattern.

> [!NOTE]
> In a real-world application, keyboard presses would be replaced by control logic in Bonsai to convert the Harp example workflows into experimental workflows. For instance, a [`Timer`] can be used instead of [`KeyDown`] to trigger the command at a set time after the workflow starts.

## Read events from device

Other blocks are used to read events from the device, select and decode specific messages, and display their contents.

![Receive Events from Device](../images/workflowbasics-receiveevents.svg)

The block is made up of:

- A [`SubscribeSubject`] operator that subscribes to the `Device Events` message stream established in the device pattern.
- A [`Parse`] operator that picks out the messages corresponding to a specific register and decodes its values.
- A [`VisualizerWindow`] that automatically opens a window and displays the decoded values when the workflow starts.

Sections that read events from devices also include a text sample of the visualizer display such as the one shown below, along with a description of what the values mean:

```text
2@15346.973344
```

## Additional resources

For more, see:

- The Bonsai-Harp interface package [documentation](https://harp-tech.org/articles/operators.html) to learn more about Harp operators.
- The Harp [website](https://harp-tech.org/protocol/BinaryProtocol-8bit.html) for more information on how Harp works.
- The Bonsai [documentation](https://bonsai-rx.org/docs/) to learn more about Bonsai control logic.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`BehaviorSubject`]: xref:Bonsai.Reactive.BehaviorSubject
[`CreateMessage`]: xref:Bonsai.Harp.CreateMessage
[`Device`]: xref:Bonsai.Harp.Device
[`DeviceDataWriter`]: xref:Harp.SoundCard.DeviceDataWriter
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MessageWriter`]: xref:Bonsai.Harp.MessageWriter
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Parse`]: xref:Bonsai.Harp.Parse
[`PublishSubject`]: xref:Bonsai.Reactive.PublishSubject
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`Timer`]: xref:Bonsai.Reactive.Timer
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
