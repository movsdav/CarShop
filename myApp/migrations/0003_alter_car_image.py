# Generated by Django 4.1.3 on 2022-12-04 12:35

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('myApp', '0002_car_image'),
    ]

    operations = [
        migrations.AlterField(
            model_name='car',
            name='image',
            field=models.ImageField(default='no-image-product.jpg', upload_to=''),
        ),
    ]
