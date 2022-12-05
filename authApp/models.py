from django.db import models

from django.contrib.auth.models import User


# Create your models here.
class UserProfile(models.Model):
    first_name = models.CharField(max_length=32, default='Undefined')
    last_name = models.CharField(max_length=32, default='Undefined')
    avatar = models.ImageField(upload_to='images/users', default='static/images/no-user-avatar.png')
    user = models.OneToOneField(User, on_delete=models.CASCADE, primary_key=True)

    def __str__(self):
        return self.user.username
