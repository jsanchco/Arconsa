import { DatePickerComponent } from '@syncfusion/ej2-react-calendars';
import React, { Component } from 'react';

import { loadCldr, L10n } from "@syncfusion/ej2-base";

import gregorian from 'cldr-data/main/es/ca-gregorian.json';
import numbers from 'cldr-data/main/es/numbers.json';
import currencyData from 'cldr-data/main/es/currencies.json';
import timeZoneNames from 'cldr-data/main/es/timeZoneNames.json';
import numberingSystems from 'cldr-data/supplemental/numberingSystems.json';
import weekData from 'cldr-data/supplemental/weekData.json';

loadCldr(numberingSystems, currencyData, gregorian, numbers, timeZoneNames, weekData);
  
L10n.load({
  'es': {
    datepicker: {
      "placeholder": "Por favor seleccione una fecha",
      "today": "hoy"
      }
  }
});

class Blank extends Component {
    render() {
        return <DatePickerComponent id="datepicker" locale="es"/>
      }
}

export default Blank;