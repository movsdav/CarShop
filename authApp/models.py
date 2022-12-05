from django.contrib.auth.models import User
from django.core.validators import MinValueValidator
from django.db import models

from myApp.models import Car


# Create your models here.
class UserProfile(models.Model):
    first_name = models.CharField(max_length=32, default='Undefined')
    last_name = models.CharField(max_length=32, default='Undefined')
    balance = models.IntegerField(validators=[
        MinValueValidator(0),
    ], default=0)
    avatar = models.ImageField(upload_to='images/users', default='static/images/no-user-avatar.png')
    watch_list = models.ManyToManyField(Car, null=True)
    user = models.OneToOneField(User, on_delete=models.CASCADE, primary_key=True)

    def __str__(self):
        return self.user.username
