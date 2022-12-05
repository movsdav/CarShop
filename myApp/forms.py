from crispy_forms.helper import FormHelper
from crispy_forms.layout import Submit, Layout, Row, Column
from django import forms
from .models import Car


class ManipulateProductForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        action = kwargs.pop('action', 'Submit')
        super(ManipulateProductForm, self).__init__(*args, **kwargs)
        self.helper = FormHelper(self)
        self.helper.add_input(Submit('submit', f'{action}', css_class='btn btn-primary'))

    class Meta:
        model = Car
        exclude = ['quantity']
