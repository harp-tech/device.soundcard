import WorkflowContainer from "./workflow.js"

export default {
    defaultTheme: 'light',
    iconLinks: [{
        icon: 'github',
        href: 'https://github.com/harp-tech/device.soundcard',
        title: 'GitHub'
    }],
    start: () => {
        WorkflowContainer.init();
    }
}
