import React, {Component, useContext} from 'react';
import Grid from '@material-ui/core/Grid';
import ToggleButton from '@material-ui/lab/ToggleButton';
import ToggleButtonGroup from '@material-ui/lab/ToggleButtonGroup';

export default class ViewSelect extends Component {
    state = {}
 
    handleChange = (event, newView) => {
        this.props.selectedView(newView);
    };


  children = [
    <ToggleButton key="FamilyTree" value="FamilyTree">
      FamilyTree
    </ToggleButton>,
    <ToggleButton key="Detail" value="Detail">
      Detail
    </ToggleButton>,
    <ToggleButton key="Edit" value="Edit">
      Edit
    </ToggleButton>,
    <ToggleButton key="Upload" value="Upload">
      Upload
    </ToggleButton>,
    <ToggleButton key="GetImage" value="GetImage">
      Get Image
    </ToggleButton>
  ];

  render () {
      
      return (
        <Grid item>
        <ToggleButtonGroup size="small" value={this.props.activeView} exclusive onChange={this.handleChange}>
          {this.children}
        </ToggleButtonGroup>
      </Grid> );
  }
      
    
  
}