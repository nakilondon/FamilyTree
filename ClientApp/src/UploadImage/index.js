import React from 'react';
import ImageUploader from 'react-images-upload';
import axios, {post} from 'axios';
import Grid from '@material-ui/core/Grid';
import { TextField } from '@material-ui/core';
import Button from '@material-ui/core/Button'
 
export default class Upload extends React.Component {
 
    constructor(props) {
        super(props);
         this.state = { picture: null, details: [] };
         this.onDrop = this.onDrop.bind(this);
    }

    onChange = (e) => {
      const valueData = {[e.target.id]: e.target.value};
      const data = {...this.state.details, ...valueData };
      this.setState({details:data});
    }

    onDrop(picture) {
        this.setState({
            picture: picture,
        });
        
    }

    uploadHandler = () => {
        const formData = new FormData()
        formData.append(
          'image',
          this.state.picture
        )

        axios.post('familytree/Upload', formData);
      }

      submit = (e) => {
        e.preventDefault();
    
        const url = `familytree/upload`;
        const formData = new FormData();
        formData.append('description', this.state.details.description)
        formData.append('file', this.state.picture[0]);
        const config = {
          headers: {
            'content-type': 'multipart/form-data',
          },
        };
        return post(url, formData, config);
      }
 
    render() {
        return (
          <Grid container direction="column" justify="center" alignItems="center">
            <Grid item>
              <form onSubmit={this.submit}>
                <Grid item>
                  <TextField
                    id="description"
                    label="Description"
                    onChange={this.onChange}
                  />
                </Grid>
                <Grid item>
                  <ImageUploader
                    withIcon={true}
                    buttonText='Choose images'
                    onChange={this.onDrop}
                    imgExtension={['.jpg', '.gif', '.png', '.gif']}
                    maxFileSize={5242880}
                />
                <Button variant="contained" color="primary" type="submit">
                  Submit
                </Button>
              </Grid>
            </form>
          </Grid>
        </Grid>
        );
    }
}