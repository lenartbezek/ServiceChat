package com.example.admin.servicechattm.model;

import com.example.admin.servicechattm.R;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Admin on 28.12.2016.
 */

public class DummyData {

    private static final String[] titles = {"test 1", "test 2", "test 3"};
    private static final String[] names = {"name1", "name2", "name3"};
    private static final int icon = R.drawable.ic_sentiment_very_satisfied_black_24dp;
    private static final int[] icons = {android.R.drawable.ic_popup_reminder, android.R.drawable.ic_menu_add, android.R.drawable.ic_menu_delete};

    public static List<ListItem> getListData(){

        List<ListItem> data = new ArrayList<>();
        for(int i =0; i<5; i++)
            for(int j=0; j < titles.length; j++){
                ListItem item = new ListItem();
                item.setTitle(titles[j]);
                item.setName(names[j]);


                data.add(item);
            }
        return data;
    }
}
