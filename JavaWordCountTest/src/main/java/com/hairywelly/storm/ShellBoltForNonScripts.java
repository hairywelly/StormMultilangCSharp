package com.hairywelly.storm;

import backtype.storm.task.IBolt;
import backtype.storm.task.OutputCollector;
import backtype.storm.task.ShellBolt;
import backtype.storm.task.TopologyContext;
import backtype.storm.tuple.Tuple;

import java.nio.file.Paths;
import java.util.Map;

/**
 * Created by Gareth.Smith on 20/06/2014.
 */
public class ShellBoltForNonScripts implements IBolt {
    private String[] _command;
    private IBolt _proxyOfShellBolt;

    public ShellBoltForNonScripts(String... _command) {
        this._command = _command;
    }

    @Override
    public void prepare(Map stormConf, TopologyContext context, OutputCollector collector) {
        if(_command.length >0){
            _command[0]=Paths.get(context.getCodeDir(),_command[0]).toString();
        }
        _proxyOfShellBolt = new ShellBolt(_command);
        _proxyOfShellBolt.prepare(stormConf,context,collector);
    }

    @Override
    public void execute(Tuple input) {
        _proxyOfShellBolt.execute(input);
    }

    @Override
    public void cleanup() {
        _proxyOfShellBolt.cleanup();
    }
}
