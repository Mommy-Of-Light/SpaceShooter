import keyboard
import time

def auto_space():
    while True:
        if keyboard.is_pressed('space'):
            keyboard.press_and_release('space')
            time.sleep(0.01)

if __name__ == "__main__":
    try:
        auto_space()
    except KeyboardInterrupt:
        print("Auto-space stopped.")