import os

from django.db import models
from django.dispatch import receiver
from django.core.validators import MinValueValidator, MaxValueValidator


class Car(models.Model):
    CHOICES = (
        ('mb', 'Mercedes-Benz'),
        ('bw', 'BMW'),
        ('au', 'Audi'),
        ('ty', 'Toyota'),
        ('ns', 'Nissan'),
        ('mt', 'Mitsubishi'),
    )

    name = models.CharField(max_length=2, choices=CHOICES)
    model = models.CharField(max_length=32, unique=True)
    price = models.IntegerField(validators=[
        MinValueValidator(0),
    ])
    quantity = models.IntegerField(
        validators=[
            MinValueValidator(0),
            MaxValueValidator(10),
        ], default=0
    )
    image = models.ImageField(upload_to='images/products', default='static/images/no-image-product.jpg')

    def __str__(self):
        return f'{self.name} {self.model}'
