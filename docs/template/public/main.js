import WorkflowContainer from "./workflow.js"
import Navbar from "./navbar.js"

export default {
    defaultTheme: 'light',
    iconLinks: [{
        icon: 'github',
        href: 'https://github.com/harp-tech/device.soundcard',
        title: 'GitHub'
    }],
    start: () => {
        WorkflowContainer.init();
        Navbar.init();
    }
}
