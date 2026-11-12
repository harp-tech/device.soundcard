from harp.device import core, soundcard


def test_device_identity_matches_the_metadata():
    assert soundcard.DEVICE_NAME == "SoundCard"
    assert soundcard.WHO_AM_I == 1280


def test_register_map_covers_the_core_and_application_registers():
    # Application registers start at address 32, so the map is only complete if
    # the core registers were merged into it as well.
    assert soundcard.REGISTER_MAP
    assert all(address in soundcard.REGISTER_MAP for address in core.REGISTER_MAP)
    assert any(address >= 32 for address in soundcard.REGISTER_MAP)
