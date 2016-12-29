package com.example.admin.servicechattm.model;

import com.example.admin.servicechattm.R;

import java.util.ArrayList;
import java.util.List;

/**
 * Neki testni podatki, naj bi simulirali nekaj kar bi nek servis vrgu vn v jsonu
 */

public class DummyData {
    private static final String[] messages = {"testni msg 1", "testni msg 2", "testni msg 3", "testni msg 4"
    };
    private static final String[] messageAuthors = {"testni avtor 1", "testni avtor 2", "testni avtor 3", "testni avtor 4"

    };
    private static final int icon = R.drawable.ic_sentiment_very_satisfied_black_24dp;

    public static List<ListItem> getListData() {
        List<ListItem> data = new ArrayList<>();
        for (int x = 0; x < 4; x++) {
            for (int i = 0; i < messages.length; i++) {
                ListItem item = new ListItem();
                item.setTitle(messages[i]);
                item.setSubTitle(messageAuthors[i]);
                data.add(item);
            }
        }
        return data;
    }
}