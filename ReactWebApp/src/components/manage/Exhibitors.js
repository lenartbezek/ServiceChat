import React from 'react';
import {List, ListItem} from 'material-ui/List';
import Divider from 'material-ui/Divider';
import {grey400} from 'material-ui/styles/colors';
import IconButton from 'material-ui/IconButton';
import MoreVertIcon from 'material-ui/svg-icons/navigation/more-vert';
import IconMenu from 'material-ui/IconMenu';
import MenuItem from 'material-ui/MenuItem';
import AppBar from 'material-ui/AppBar';
import NavigationArrowBack from 'material-ui/svg-icons/navigation/arrow-back';

const ExhibitorsInfo = [
      {
        title: 'ExhitiborsInfo title',
        contact: 'QU SHENG',
        tel: '0086-21-68752488'
      }
];

const iconButtonElement = (
  <IconButton
    touch={true}
    tooltip="more"
    tooltipPosition="bottom-left"
  >
    <MoreVertIcon color={grey400} />
  </IconButton>
);

const rightIconMenu = (
  <IconMenu iconButtonElement={iconButtonElement}>
    <MenuItem>取消参展</MenuItem>
    <MenuItem>删除</MenuItem>
  </IconMenu>
);

const Exhibitors = () => (
  <div>
      <AppBar
        title="appbar title"
        iconElementLeft={<IconButton><NavigationArrowBack /></IconButton>}
      />
      <List>
        {
          ExhibitorsInfo.map((exhibotor, i) => (
            <div key={i}>
              <ListItem
                rightIconButton={rightIconMenu}
                primaryText={exhibotor.title}
                secondaryText={
                  <p>
                    <span>contact：</span><span>{exhibotor.contact}</span><br />
                    <span>tel：</span><span>{exhibotor.tel}</span>
                  </p>
                }
                secondaryTextLines={2}
              />
              <Divider inset={true} />
            </div>
          ))
        }
      </List>
  </div>
);

export default Exhibitors;