# Generated by Django 4.1.3 on 2022-12-05 15:00

import django.core.validators
from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('myApp', '0017_alter_car_image'),
    ]

    operations = [
        migrations.AddField(
            model_name='car',
            name='quantity',
            field=models.IntegerField(default=0, validators=[django.core.validators.MinValueValidator(0), django.core.validators.MaxValueValidator(10)]),
        ),
    ]
