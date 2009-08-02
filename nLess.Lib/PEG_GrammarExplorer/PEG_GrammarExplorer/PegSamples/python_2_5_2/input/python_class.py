from abc import ABCMeta

class Drawable():
    __metaclass__ = ABCMeta

    def draw(self, x, y, scale=1.0):
        pass

    def draw_doubled(self, x, y):
        self.draw(x, y, scale=2.0)
